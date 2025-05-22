import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { Order, SetProduction, SetProductionResponse } from '../../models';
import { formatDate } from '@angular/common';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-production-form',
  templateUrl: './production-form.component.html',
  styleUrls: ['./production-form.component.css']
})
export class ProductionFormComponent implements OnInit, OnDestroy {
  form: FormGroup;
  orders: Order[] = [];
  selectedOrder: Order | null = null;
  startTime: number | null = null;
  calculatedCycleTime: number = 0;
  requiredCycleTime: number = 30; // Fixed at 30 seconds per requirement
  responseMessage: string | null = null;
  responseType: string | null = null;
  private timerSubscription: Subscription | null = null;

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.group({
      email: ['teste@sqr.com.br', [Validators.required, Validators.email]],
      orderId: ['', Validators.required],
      productionDate: ['', Validators.required],
      quantity: [0, [Validators.required, Validators.min(1)]],
      materialCode: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.apiService.getOrders().subscribe({
      next: (data) => {
        console.log('Orders received:', data);
        this.orders = data.orders;
      },
      error: (err) => console.error('Error fetching orders:', err)
    });
  }

  onOrderChange(): void {
    const orderId = this.form.get('orderId')?.value;
    console.log('Order selected:', orderId);
    this.selectedOrder = this.orders.find(o => o.order === orderId) || null;
    this.form.get('materialCode')?.setValue('');
    this.startTime = Date.now();
    this.calculatedCycleTime = 0;
    console.log('Start time set:', this.startTime);

    // Stop any existing timer
    if (this.timerSubscription) {
      this.timerSubscription.unsubscribe();
    }

    // Start new timer
    this.timerSubscription = interval(1000).subscribe(() => {
      console.log('Timer tick');
      this.calculateCycleTime();
      console.log('Cycle time updated:', this.calculatedCycleTime);
      this.cdr.detectChanges();
    });
  }

  calculateCycleTime(): void {
    if (this.startTime) {
      this.calculatedCycleTime = (Date.now() - this.startTime) / 1000;
    } else {
      this.calculatedCycleTime = 0;
    }
  }

  isSubmitEnabled(): boolean {
    const enabled = this.form.valid && this.calculatedCycleTime >= this.requiredCycleTime;
    console.log('Submit enabled:', enabled, 
                'Form valid:', this.form.valid, 
                'Cycle time:', this.calculatedCycleTime, 
                'Required:', this.requiredCycleTime,
                'Form errors:', this.form.errors,
                'Email valid:', this.form.get('email')?.valid, 
                'OrderId valid:', this.form.get('orderId')?.valid,
                'ProductionDate valid:', this.form.get('productionDate')?.valid,
                'Quantity valid:', this.form.get('quantity')?.valid,
                'MaterialCode valid:', this.form.get('materialCode')?.valid);
    return enabled;
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.calculateCycleTime();
      const data: SetProduction = {
        email: this.form.get('email')?.value,
        order: this.form.get('orderId')?.value,
        productionDate: formatDate(this.form.get('productionDate')?.value, 'yyyy-MM-dd', 'en-US'),
        productionTime: formatDate(new Date(), 'HH:mm:ss', 'en-US'),
        quantity: this.form.get('quantity')?.value,
        materialCode: this.form.get('materialCode')?.value,
        cycleTime: this.calculatedCycleTime
      };

      this.apiService.setProduction(data).subscribe({
        next: (response: SetProductionResponse) => {
          this.responseMessage = response.description;
          this.responseType = response.type;
          if (response.status === 200) {
            this.form.reset({ email: 'teste@sqr.com.br' });
            this.selectedOrder = null;
            this.form.get('orderId')?.setValue(''); // Explicitly clear orderId
            this.form.get('productionDate')?.setValue('');
            this.form.get('quantity')?.setValue(0);
            this.form.get('materialCode')?.setValue('');
            this.startTime = null;
            this.calculatedCycleTime = 0;
            if (this.timerSubscription) {
              this.timerSubscription.unsubscribe();
            }
          }
        },
        error: (err) => {
          this.responseMessage = 'Erro ao enviar apontamento';
          this.responseType = 'E';
          console.error('Error submitting production:', err);
        }
      });
    }
  }

  ngOnDestroy(): void {
    if (this.timerSubscription) {
      this.timerSubscription.unsubscribe();
    }
  }
}