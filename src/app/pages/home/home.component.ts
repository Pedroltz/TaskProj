import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { NavbarComponent } from '../../components/navbar/navbar.component';

interface Task {
  id?: string;
  title: string;
  description: string;
  dueDate: string;
  priority: number;
  status: string;
}

interface User {
  id: string;
  name: string;
  email: string;
  tasks: Task[];
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  newTask: Task = {
    title: '',
    description: '',
    dueDate: '',
    priority: 2,
    status: 'pending'
  };

  tasks: Task[] = [];
  showForm: boolean = false;
  editingTaskId: string | null = null;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  toggleForm(): void {
    this.showForm = !this.showForm;
    if (!this.showForm) this.cancelEdit();
  }

  loadTasks(): void {
    const userId = localStorage.getItem('userId');
    if (!userId) return;

    this.http.get<User>(`http://localhost:5261/api/users/${userId}`).subscribe({
      next: user => this.tasks = user.tasks || [],
      error: err => console.error('[ERRO] ao buscar tarefas:', err)
    });
  }

  createTask(): void {
    const userId = localStorage.getItem('userId');
    if (!userId) return;

    this.http.post(`http://localhost:5261/api/users/${userId}/tasks`, this.newTask).subscribe({
      next: () => {
        this.resetForm();
        this.loadTasks();
      },
      error: err => console.error('[ERRO] ao criar tarefa:', err)
    });
  }

  updateTask(taskId: string): void {
    const userId = localStorage.getItem('userId');
    if (!userId) return;

    this.http.put(`http://localhost:5261/api/users/${userId}/tasks/${taskId}`, this.newTask).subscribe({
      next: () => {
        this.resetForm();
        this.loadTasks();
      },
      error: err => console.error('[ERRO] ao atualizar tarefa:', err)
    });
  }

  editTask(task: Task): void {
    this.editingTaskId = task.id || null;
    this.newTask = { ...task };
    this.showForm = true;
  }

  cancelEdit(): void {
    this.editingTaskId = null;
    this.resetForm();
  }

  resetForm(): void {
    this.newTask = {
      title: '',
      description: '',
      dueDate: '',
      priority: 2,
      status: 'pending'
    };
    this.showForm = false;
  }

  deleteTask(taskId: string): void {
    const userId = localStorage.getItem('userId');
    if (!userId) return;

    this.http.delete(`http://localhost:5261/api/users/${userId}/tasks/${taskId}`).subscribe({
      next: () => this.tasks = this.tasks.filter(task => task.id !== taskId),
      error: err => console.error('[ERRO] ao excluir tarefa:', err)
    });
  }

  changeStatus(task: Task, status: string): void {
    const userId = localStorage.getItem('userId');
    if (!userId || !task.id) return;

    const updated = { ...task, status };

    this.http.put(`http://localhost:5261/api/users/${userId}/tasks/${task.id}`, updated).subscribe({
      next: () => this.loadTasks(),
      error: err => console.error('[ERRO] ao alterar status:', err)
    });
  }
}
