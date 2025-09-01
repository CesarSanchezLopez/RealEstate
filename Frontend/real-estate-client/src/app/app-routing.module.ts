import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OwnerComponent } from './components/owner/owner.component';
import { PropertyComponent } from './components/property/property.component';

const routes: Routes = [
  { path: 'owners', component: OwnerComponent },
  { path: 'properties', component: PropertyComponent },
  { path: '', redirectTo: '/owners', pathMatch: 'full' }, // home por defecto
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }