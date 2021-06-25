import { HttpClient, HttpEventType } from '@angular/common/http';
import { ReturnStatement } from '@angular/compiler';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { IMazeCell } from './interface/MazeCell';

import { LoggingService } from './logging/logging.service';
import { UploadService } from './services/upload-service/upload.service';
import { StuffService } from './stuff/stuff.service';

@Component({
  selector: 'valant-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less'],
})
export class AppComponent implements OnInit {
  public title = 'Valant demo';
  public data: string[];
  public progress: number;
  public message: string;
  public mazenames: any;
  public maze: IMazeCell[][];
  public isMazeLoaded: boolean = false;
  public mazeTitle: string = 'Please load a maze or upload a maze file (.txt)';
  @Output() public onUploadFinished = new EventEmitter();

  constructor(
    private logger: LoggingService,
    private stuffService: StuffService,
    private http: HttpClient,
    private uploadService: UploadService
  ) {}

  ngOnInit() {
    this.logger.log('Welcome to the AppComponent');
    this.getStuff();
    this.getMezeNames();
  }

  private getStuff(): void {
    this.stuffService.getStuff().subscribe({
      next: (response: string[]) => {
        this.data = response;
      },
      error: (error) => {
        this.logger.error('Error getting stuff: ', error);
      },
    });
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }

    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.http
      .post('http://localhost:47181/maze/Upload', formData, { reportProgress: true, observe: 'events' })
      .subscribe((event) => {
        if (event.type === HttpEventType.UploadProgress) this.progress = Math.round((100 * event.loaded) / event.total);
        else if (event.type === HttpEventType.Response) {
          this.message = 'Upload success.';
          this.onUploadFinished.emit(event.body);
          this.getMezeNames();
        }
      });
  };

  public getMezeNames() {
    this.uploadService.getAllMazeNames().subscribe(
      (data) => {
        this.mazenames = data;
        console.log(this.mazenames);
      },
      (error) => {
        console.log(error);
      }
    );
  }

  public async loadMaze(fileName: string) {
    this.uploadService.loadMaze(fileName).subscribe(
      (data) => {
        this.maze = data;
        this.isMazeLoaded = true;
        this.getCurrentMaze();
        this.isWall = false;
        console.log(this.maze);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  public currentMaze: IMazeCell;
  public isEnd: boolean = false;
  public getCurrentMaze() {
    this.uploadService.getCurrentMaze().subscribe(
      (data) => {
        this.currentMaze = data;
        console.log(this.currentMaze);
        if (this.currentMaze.symbol == 'E') {
          this.isEnd = true;
          alert('Congratulation! You have reach the End!');
          this.maze = null;
          this.isMazeLoaded = false;
        }
      },
      (error) => {
        console.log(error);
      }
    );
  }

  public isWall: boolean;
  public goEast() {
    console.log(this.currentMaze);
    this.uploadService.goEast(this.currentMaze).subscribe(
      (data) => {
        if (data != false) {
          this.maze = data;
          this.getCurrentMaze();
          this.isWall = false;
        } else {
          this.isWall = true;
        }
      },
      (error) => {
        console.log(error);
      }
    );
  }
  public goWest() {
    console.log(this.currentMaze);
    this.uploadService.goWest(this.currentMaze).subscribe(
      (data) => {
        if (data != false) {
          this.maze = data;
          this.getCurrentMaze();
          this.isWall = false;
        } else {
          this.isWall = true;
        }
      },
      (error) => {
        console.log(error);
      }
    );
  }

  public goNorth() {
    console.log(this.currentMaze);
    this.uploadService.goNorth(this.currentMaze).subscribe(
      (data) => {
        if (data != false) {
          this.maze = data;
          this.getCurrentMaze();
          this.isWall = false;
        } else {
          this.isWall = true;
        }
      },
      (error) => {
        console.log(error);
      }
    );
  }

  public goSouth() {
    console.log(this.currentMaze);
    this.uploadService.goSouth(this.currentMaze).subscribe(
      (data) => {
        if (data != false) {
          this.maze = data;
          this.getCurrentMaze();
          this.isWall = false;
        } else {
          this.isWall = true;
        }
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
