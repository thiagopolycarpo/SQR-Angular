import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductionFormComponent } from './components/production-form/production-form.component';
import { ProductionListComponent } from './components/production-list/production-list.component';

const routes: Routes = [
  { path: '', component: ProductionFormComponent },
  { path: 'productions', component: ProductionListComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }