import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { SelectModule } from 'primeng/select';
import { ImportsModule } from '../../imports';
import { AttendanceService } from '../../service/attendance-service';

interface Month {
  name: string;
  id: number;
}

@Component({
  selector: 'app-statistics',
  imports: [ImportsModule],
  templateUrl: './statistics.component.html',
  styleUrl: './statistics.component.scss'
})
export class StatisticsComponent {

  month: Month[] | undefined;
  selectedmonth: any;
  selectedYear: any;
  year: any[] | undefined;

  constructor(private attendanceService:AttendanceService){}
  ngOnInit(): void {
    this.month = [
      { name: 'January', id: 1 },
      { name: 'February', id: 2 },
      { name: 'March', id: 3 },
      { name: 'April', id: 4 },
      { name: 'May', id: 5 },
      { name: 'June', id: 6 },
      { name: 'July', id: 7 },
      { name: 'August', id: 8 },
      { name: 'September', id: 9 },
      { name: 'October', id: 10 },
      { name: 'November', id: 11 },
      { name: 'December', id: 12 }
    ];
    this.year = [
      { year: 2025 },
    ]
  }
  print() {
    this.attendanceService.exportAttendance(this.selectedYear, this.selectedmonth)
      .subscribe((res: Blob) => {
        const blob = new Blob([res], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);
        window.open(url); // Opens in a new tab
      }, error => {
        console.error('Failed to load PDF', error);
      });
  }
  
  
}
