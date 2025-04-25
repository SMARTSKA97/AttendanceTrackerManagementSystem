import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { SelectModule } from "primeng/select";
import { MenubarModule } from 'primeng/menubar';
import { AvatarModule } from 'primeng/avatar'

@NgModule({
    imports: [
        ButtonModule,
        CardModule,
        CommonModule,
        SelectModule,
        FormsModule,
        MenubarModule,
        AvatarModule
    ],
      exports: [
        ButtonModule,
        CardModule,
        CommonModule,
        SelectModule,
        FormsModule,
        MenubarModule,
        AvatarModule
      ],
  providers: [  ]
})
export class ImportsModule {}
