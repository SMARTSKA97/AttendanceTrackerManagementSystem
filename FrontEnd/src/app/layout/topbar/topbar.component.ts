import { Component } from '@angular/core';
import { MegaMenuItem } from 'primeng/api';
import { ImportsModule } from '../../imports';

@Component({
  selector: 'app-topbar',
  imports: [ImportsModule],
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.scss'
})
export class TopbarComponent {
  items: MegaMenuItem[] | undefined;
  layoutService: any;



  ngOnInit() {
    this.items = [
      {
        label: 'Dashboard',
        icon: 'pi pi-fw pi-home',
        routerLink: '/dashboard',
      },
      {
        label: 'Statistics',
        icon: 'pi pi-fw pi-book',
        routerLink: '/statistics'
      }
    ];
  }
}

