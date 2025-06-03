import { DatePipe, KeyValue } from '@angular/common';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, Subscription } from 'rxjs';
import { ApiResponse, StatusFlags, eMessageType } from '../models/data.model';

@Injectable({ providedIn: 'root' })
export class DataService {
  notify: Subject<KeyValue<string, string>> = new Subject();

  constructor(public http: HttpClient) { }

  get<T>(url: string, data?: string): Promise<ApiResponse<T>> {
    return new Promise<ApiResponse<T>>((resolve) => {
      var getSubscription: Subscription = this.http.get<ApiResponse<T>>(data ? `${url}/${data}` : url).subscribe({
        next: (response) => resolve(this.completeResponse(response)),
        error: (error) => this.notify.next({ key: eMessageType.Error, value: error }),
        complete: () => getSubscription.unsubscribe()
      });
    });
  }

  getData<T>(url: string, data?: string): Promise<T> {
    return new Promise<T>(async (resolve) => resolve((await this.get<T>(url, data)).data));
  }

  post<T>(url: string, data: any): Promise<ApiResponse<T>> {
    return new Promise<ApiResponse<T>>((resolve) => {
      var postSubscription: Subscription = this.http.post<ApiResponse<T>>(url, data).subscribe({
        next: (response) => resolve(this.completeResponse(response)),
        error: (error) => this.notify.next({ key: eMessageType.Error, value: error }),
        complete: () => postSubscription.unsubscribe()
      });
    });
  }

  postData<T>(url: string, data?: any): Promise<T> {
    return new Promise<T>(async (resolve) => resolve((await this.post<T>(url, data)).data));
  }

  postFile(url: string, file: any): Observable<any> {
    const formData: FormData = new FormData(); formData.append('files', file, file.name);
    return this.http.post(url, formData, { reportProgress: true, observe: 'events' });
  }

  delete<T>(url: string, data: any): Promise<ApiResponse<T>> {
    return new Promise<ApiResponse<T>>((resolve) => {
      var deleteSubscription: Subscription = this.http.delete<ApiResponse<T>>(`${url}/${data}`).subscribe({
        next: (response) => resolve(this.completeResponse<T>(response)),
        error: (error) => this.notify.next({ key: eMessageType.Error, value: error }),
        complete: () => deleteSubscription.unsubscribe()
      });
    });
  }

  downloadFile(url: string, data: any, fileName = ''): void {
    var downloadSubscription: Subscription = this.http.post(url, data, { responseType: 'blob', observe: 'response' }).subscribe({
      next: (response: HttpResponse<any>) => {
        const link: any = document.createElement('a');
        link.href = window.URL.createObjectURL(response.body);
        link.download = fileName ? fileName : new DatePipe('en-US').transform(new Date(), 'ddMMyyyyHHmmss');
        link.click();
      },
      error: (error) => this.notify.next({ key: eMessageType.Error, value: error }),
      complete: () => downloadSubscription.unsubscribe()
    });
  }

  completeResponse<T>(apiResponse: any): ApiResponse<T> {
    if (apiResponse && apiResponse.message) {
      switch (apiResponse.status) {
        // Success
        case StatusFlags.Success: {
          this.notify.next({ key: eMessageType.Success, value: apiResponse.message });
          break;
        }
        // Exceptions
        case StatusFlags.Failed: {
          this.notify.next({ key: eMessageType.Error, value: apiResponse.message });
          break;
        }
        // Warnings
        case StatusFlags.AlreadyExists:
        case StatusFlags.DependencyExists: {
          this.notify.next({ key: eMessageType.Warning, value: apiResponse.message });
          break;
        }
        // Information
        default: {
          this.notify.next({ key: eMessageType.Info, value: apiResponse.message });
          break;
        }
      }
    }
    return apiResponse;
  }
}
