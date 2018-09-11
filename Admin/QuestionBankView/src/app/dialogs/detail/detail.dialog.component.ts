import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {Component, Inject, Output, EventEmitter} from '@angular/core';
import {QuestionService} from '../../../service/question.service';
import {FormControl, Validators} from '@angular/forms';
import {IMCQ} from '../../../service/mcq';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import {FormBuilder, FormGroup,FormArray} from "@angular/forms";

export interface Fruit {
  name: string;
}

@Component({
  selector: 'app-detail.dialog',
  templateUrl: '../../dialogs/detail/detail.dialog.html',
  styleUrls: ['../../dialogs/detail/detail.dialog.css']
})

export class DetailDialogComponent {
  createForm:FormGroup;
  @Output() editedQuestion = new EventEmitter<IMCQ>();
  constructor(public dialogRef: MatDialogRef<DetailDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data,
              public dataService: QuestionService) {}

  formControl = new FormControl('', [
    Validators.required
    // Validators.email,
  ]);

  SaveData(data){
    this.dataService.editQuestions(data.questionId, data).subscribe(result => {
      if(result.statusText == "OK")
      {
        this.editedQuestion.emit(data);
      }
    });
    this.dialogRef.close();
  }
  }
