import { HttpClient, HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IMazeCell } from '../../interface/MazeCell';

@Injectable({
  providedIn: 'root',
})
export class UploadService {
  constructor(private http: HttpClient) {}

  uploadFile(formData: FormData): void {
    this.http.post('http://localhost:47181/maze', formData, { reportProgress: true, observe: 'events' });
  }

  getAllMazeNames(): Observable<any> {
    return this.http.get('http://localhost:47181/maze/getFileNames/').pipe(catchError(this.errorHandler));
  }

  loadMaze(fileName: string): Observable<any> {
    return this.http.get('http://localhost:47181/maze/loadMaze/' + fileName).pipe(catchError(this.errorHandler));
  }

  getCurrentMaze(): Observable<any> {
    return this.http.get('http://localhost:47181/maze/getCurrentMaze/').pipe(catchError(this.errorHandler));
  }

  goEast(mc: IMazeCell): Observable<any> {
    return this.http.post('http://localhost:47181/maze/GoEast/', mc).pipe(catchError(this.errorHandler));
  }

  goWest(mc: IMazeCell): Observable<any> {
    return this.http.post('http://localhost:47181/maze/GoWest/', mc).pipe(catchError(this.errorHandler));
  }

  goNorth(mc: IMazeCell): Observable<any> {
    return this.http.post('http://localhost:47181/maze/GoNorth/', mc).pipe(catchError(this.errorHandler));
  }

  goSouth(mc: IMazeCell): Observable<any> {
    return this.http.post('http://localhost:47181/maze/GoSouth/', mc).pipe(catchError(this.errorHandler));
  }
  errorHandler(error: HttpErrorResponse) {
    console.error(error);
    return throwError(error.message || 'Server Error');
  }
}
