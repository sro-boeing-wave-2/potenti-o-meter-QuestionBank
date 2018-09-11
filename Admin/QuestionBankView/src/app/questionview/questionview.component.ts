import { Component, OnInit, ViewChild, Input} from '@angular/core';
import { QuestionService } from '../../service/question.service';
import {MatPaginator, MatTableDataSource, MatDialog} from '@angular/material';
import { IMCQ } from '../../service/mcq';
import { AddDialogComponent } from '../dialogs/add/add.dialog.component';
import { DetailDialogComponent } from '../dialogs/detail/detail.dialog.component';
import { DeleteDialogComponent } from '../dialogs/delete/delete.dialog.component';

@Component({
  selector: 'app-questionview',
  templateUrl: './questionview.component.html',
  styleUrls: ['./questionview.component.css']
})
export class QuestionviewComponent implements OnInit {
  public Questions = [];
  public isDiabled: boolean = true;

  constructor(private _questionService: QuestionService, public dialog: MatDialog) { }
  displayedColumns: string[] = ['position', 'name', 'weight', 'symbol', 'actions'];
  dataSource = new MatTableDataSource<IMCQ>(this.Questions);

  @ViewChild(MatPaginator) paginator: MatPaginator;

  ngOnInit() {
    this._questionService.getQuestions()
    .subscribe(data => {this.Questions.push(...data.json()); this.dataSource.paginator = this.paginator;});;
  }

  addNew(issue: IMCQ) {
    let dialogRef = this.dialog.open(AddDialogComponent, {
      data: {issue}
  });
  }

  selectRow(row) {
    var isDiabled = this.isDiabled;
    let dialogRef = this.dialog.open(DetailDialogComponent ,{
      data: {row, isDiabled}
    })
    console.log(row);
  }

  startEdit(row) {
    var isDiabled =!this.isDiabled;
    let dialogRef = this.dialog.open(DetailDialogComponent ,{
      data: {row, isDiabled}
    })
    const sub = dialogRef.componentInstance.editedQuestion.subscribe((result) => {
      this.dataSource.paginator = this.paginator
    });
  }

  deleteItem(row: IMCQ) {
    var index = this.Questions.indexOf(row);
    let dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {index, row}
    });
    const sub = dialogRef.componentInstance.deletedQuestion.subscribe((result) => {
      var index = this.Questions.indexOf(result);
      if (index > -1) {
        this.Questions.splice(index, 1);
      }
      this.dataSource.paginator = this.paginator
    });
  }
}
