import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {Component, Inject, Output, EventEmitter} from '@angular/core';
import {QuestionService} from '../../../service/question.service';
import { IMCQ } from '../../../service/mcq';


@Component({
  selector: 'app-delete.dialog',
  templateUrl: '../../dialogs/delete/delete.dialog.html',
  styleUrls: ['../../dialogs/delete/delete.dialog.css']
})
export class DeleteDialogComponent {
  @Output() deletedQuestion = new EventEmitter<IMCQ>();
  constructor(public dialogRef: MatDialogRef<DeleteDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any, public dataService: QuestionService) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

  confirmDelete(): void {
    this.dataService.deletetQuestions(this.data.row.questionId).subscribe(result =>
    {if(result.statusText == "OK")
    {
      this.deletedQuestion.emit(this.data.row);
    }
    });
  }
}
