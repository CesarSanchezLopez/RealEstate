import { Component, OnInit } from '@angular/core';
import { Owner, OwnerService } from '../../services/owner.service';

@Component({
  selector: 'app-owner',
  templateUrl: './owner.component.html',
  styleUrls: ['./owner.component.css']
})
export class OwnerComponent implements OnInit {
  owners: Owner[] = [];
  newOwner: Owner = { idOwner: 0, name: '', address: '', birthday: '' };

  constructor(private ownerService: OwnerService) {}

  ngOnInit(): void {
    this.loadOwners();
  }

  loadOwners(): void {
    this.ownerService.getOwners().subscribe({
      next: (data) => (this.owners = data),
      error: (err) => console.error('Error cargando owners', err)
    });
  }

  addOwner(): void {
    this.ownerService.createOwner(this.newOwner).subscribe({
      next: (created) => {
        this.owners.push(created);
        this.newOwner = { idOwner: 0, name: '', address: '', birthday: '' };
      },
      error: (err) => console.error('Error creando owner', err)
    });
  }

  deleteOwner(id: number): void {
    this.ownerService.deleteOwner(id).subscribe({
      next: () => {
        this.owners = this.owners.filter(o => o.idOwner !== id);
      },
      error: (err) => console.error('Error eliminando owner', err)
    });
  }
}