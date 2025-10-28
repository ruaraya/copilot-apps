    import { NgModule } from '@angular/core';
    import { MatButtonModule } from '@angular/material/button';
    import { MatCardModule } from '@angular/material/card';
import { MatLabel } from '@angular/material/form-field';
    // Import other modules as needed

    @NgModule({
      imports: [
        MatButtonModule,
        MatCardModule,
        MatLabel
      ],
      exports: [
        MatButtonModule,
        MatCardModule,
        MatLabel
      ]
    })
    export class MaterialModule { }