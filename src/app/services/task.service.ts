// src/app/services/task.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface TaskItem {
  id?: string;
  title: string;
  description?: string;
  dueDate?: string;
  priority: number;
  status: string;
  userId?: string;
}

export interface UserWithTasks {
  id: string;
  name: string;
  email: string;
  tasks: TaskItem[];
}

@Injectable({ providedIn: 'root' })
export class TaskService {
  private baseUrl = 'http://localhost:5261/api';

  constructor(private http: HttpClient) {}

  getUserWithTasks(userId: string): Observable<UserWithTasks> {
    return this.http.get<UserWithTasks>(`${this.baseUrl}/users/${userId}`);
  }

  createTaskForUser(userId: string, payload: Omit<TaskItem, 'id'>): Observable<TaskItem> {
    return this.http.post<TaskItem>(
      `${this.baseUrl}/users/${userId}/tasks`,
      payload
    );
  }
}
