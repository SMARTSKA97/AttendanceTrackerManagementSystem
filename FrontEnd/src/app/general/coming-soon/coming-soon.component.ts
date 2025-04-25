import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ImportsModule } from '../../imports';


@Component({
  selector: 'app-coming-soon',
  imports: [ImportsModule],
  templateUrl: './coming-soon.component.html',
  styleUrl: './coming-soon.component.scss',
  providers: [MessageService],
  standalone: true
})
export class ComingSoonComponent {
goToDashboard() {
  this.router.navigate(['/dashboard']);
}
  email: string = '';

  constructor(private messageService: MessageService, private router: Router) {}

  notifyUser() {
    if (this.email.trim()) {
      this.messageService.add({ severity: 'success', summary: 'Subscribed!', detail: `We'll notify you at ${this.email}` });
      this.email = '';
    } else {
      this.messageService.add({ severity: 'error', summary: 'Oops!', detail: 'Please enter a valid email' });
    }
  }
}
