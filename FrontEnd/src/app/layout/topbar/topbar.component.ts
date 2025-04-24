import { Component } from '@angular/core';
import { ImportsModule } from '../../assets/imports';
import { MegaMenuItem } from 'primeng/api';

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
