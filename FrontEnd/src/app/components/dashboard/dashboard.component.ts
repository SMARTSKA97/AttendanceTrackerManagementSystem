import { Component } from '@angular/core';
import { AttendanceService } from '../../service/attendance-service';
import { ImportsModule } from '../../imports';

@Component({
  selector: 'app-dashboard',
  imports: [ImportsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  isChecked: boolean = false;
  time: any;
  totalDuration: any;
  totalBreakDuration: any;

  constructor(private attendanceService: AttendanceService) { }

  ngOnInit() {
    this.checkToggleState();
    this.timeFinder();
  }
  timeFinder() {
    this.attendanceService.getToday().subscribe((res: any) => {
      if (this.isChecked == true) {
        console.log("checked out ");
        const latestExit = res.sessions.reduce((latest: { exitTime: string | number | Date; }, current: { exitTime: string | number | Date; }) =>
          new Date(current.exitTime) > new Date(latest.exitTime) ? current : latest
        );
        this.time = latestExit.exitTime;
      }
      else {
        console.log("checked in ");
        const latestEntry = res.sessions.reduce((latest: { entryTime: string | number | Date; }, current: { entryTime: string | number | Date; }) =>
          new Date(current.entryTime) > new Date(latest.entryTime) ? current : latest
        );
        this.time = latestEntry.entryTime;
      }
      this.totalDuration = res.totalWorkDuration;
      this.totalBreakDuration = res.totalBreakDuration;
    });
  }
  checkToggleState() {
    this.attendanceService.getStatus().subscribe((res:any) => {
      console.log(res);
      
      this.isChecked = res.isCheckedOut;
      console.log('Toggle state from service:', this.isChecked);
    });
  }
  toggleCheck() {
    this.attendanceService.toggleAttendance().subscribe((res: any) => {
      console.log(res);
      this.checkToggleState();
      this.timeFinder();
    });
  }

}
