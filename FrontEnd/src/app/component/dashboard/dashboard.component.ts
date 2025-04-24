import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ImportsModule } from '../../assets/imports';
import { AttendanceService } from '../../services/attendance.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-dashboard',
  imports: [ImportsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  providers: [MessageService]
})
export class DashboardComponent {
  checked: boolean = false;
  time: Date | undefined;
  constructor(private attendance: AttendanceService, private message: MessageService) { }

  ngOnInit() {
    this.checkStatus();
  }
  checkStatus() {
    this.attendance.getStatus().subscribe((res) => {
      console.log(res);
      this.checked = res.isCheckedOut;
    });
  }
  toggleCheck() {
    this.attendance.toggleAttendance().subscribe((res) => {
      console.log(res);
      this.checked = !this.checked;
      this.time = new Date();
      const formattedTime = this.time.toLocaleString('en-US', {
        weekday: 'long',
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: 'numeric',
        minute: '2-digit',
        hour12: true
      });
      if (this.checked) {
        this.message.add({ severity: 'success', summary: 'Checked IN', detail: `${formattedTime}` });
      }
      else {
        this.message.add({ severity: 'error', summary: 'Checked OUT', detail: `${formattedTime}` });
      }
    });
  }
}
