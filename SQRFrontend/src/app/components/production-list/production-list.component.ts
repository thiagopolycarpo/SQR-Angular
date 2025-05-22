import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Production } from '../../models';

@Component({
  selector: 'app-production-list',
  templateUrl: './production-list.component.html',
  styleUrls: ['./production-list.component.css']
})
export class ProductionListComponent implements OnInit {
  productions: Production[] = [];
  email: string = 'teste@sqr.com.br';
  displayedColumns: string[] = ['id', 'order', 'date', 'quantity', 'materialCode', 'cycleTime'];

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.loadProductions();
  }

  loadProductions(): void {
    this.apiService.getProduction(this.email).subscribe({
      next: (data) => {
        this.productions = data.productions;
      },
      error: (err) => console.error('Error fetching productions:', err)
    });
  }
}