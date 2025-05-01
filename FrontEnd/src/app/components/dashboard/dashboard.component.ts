import { Component } from '@angular/core';
import { AttendanceService } from '../../service/attendance-service';
import { ImportsModule } from '../../imports';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  imports: [ImportsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  providers: [DatePipe]
})
export class DashboardComponent {
  isChecked: boolean = false;
  time: any;
  totalDuration: any;
  totalBreakDuration: any;
  date: any;

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
        this.time = this.getRelativeTime(latestExit.exitTime);        
        this.date = this.formatDateTime(latestExit.exitTime);
      }
      else {
        console.log("checked in ");
        const latestEntry = res.sessions.reduce((latest: { entryTime: string | number | Date; }, current: { entryTime: string | number | Date; }) =>
          new Date(current.entryTime) > new Date(latest.entryTime) ? current : latest
        );
        this.time = this.getRelativeTime(latestEntry.entryTime);
        this.date = this.formatDateTime(latestEntry.exitTime);
      }
      this.totalDuration = this.formatDuration(res.totalWorkDuration);
      this.totalBreakDuration = this.formatDuration(res.totalBreakDuration);
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
  formatDuration(duration: string): string {
    const parts = duration.split(':');
    const hours = parseInt(parts[0], 10);
    const minutes = parseInt(parts[1], 10);
    const seconds = parseFloat(parts[2]);

    const formatted: string[] = [];
    if (hours > 0) formatted.push(`${hours} hour${hours !== 1 ? 's' : ''}`);
    if (minutes > 0) formatted.push(`${minutes} minute${minutes !== 1 ? 's' : ''}`);
    formatted.push(`${Math.floor(seconds)} second${Math.floor(seconds) !== 1 ? 's' : ''}`);

    return formatted.join(', ');
  }
  getRelativeTime(dateTime: string): string {
    const now = new Date();
    const past = new Date(dateTime);
    const diffMs = now.getTime() - past.getTime();
  
    const seconds = Math.floor(diffMs / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);
  
    if (seconds < 60) return 'just now';
    if (minutes < 60) return `${minutes} minute${minutes !== 1 ? 's' : ''} ago`;
    if (hours < 24) return `${hours} hour${hours !== 1 ? 's' : ''} ago`;
    return `${days} day${days !== 1 ? 's' : ''} ago`;
  }
  formatDateTime(dateTime: any): string {
    console.log(dateTime);
    
    const newDate = String(dateTime);
    console.log(newDate);
    
    const clearDate = newDate.split('T')[0];
    const date = new Date(clearDate);
    const options: Intl.DateTimeFormatOptions = {
      weekday: 'short',
      year: 'numeric',
      month: 'short',
      day: '2-digit'
    };

    return date.toLocaleString('en-US', options); // you can localize as needed
  }
}
