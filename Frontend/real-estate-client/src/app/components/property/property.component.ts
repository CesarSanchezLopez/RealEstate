import { Component } from '@angular/core';
import { PropertyService } from '../../services/property.service';
import { PropertyDto } from '../../models/property.model';
import { PropertyTraceDto } from '../../models/PropertyTrace.model';
@Component({
  selector: 'app-property',
  templateUrl: './property.component.html'
})
export class PropertyComponent {
  property: PropertyDto = {
    name: '',
    address: '',
    price: 0,
    codeInternal: '',
    year: 2023,
    idOwner: 1,
    traces: []
  };

  properties: PropertyDto[] = [];
  selectedFile: File | null = null;
  traces: PropertyTraceDto[] = [];

  constructor(private service: PropertyService) {}

  createProperty() {
    this.service.createProperty(this.property).subscribe(id => {
      alert('Property created with ID: ' + id);
      this.property.idProperty = id;
      this.listProperties();
    });
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  uploadImage(propertyId: number) {
    if (!this.selectedFile) {
      alert("Primero seleccione una imagen.");
      return;
    }

    this.service.addImage(propertyId, this.selectedFile).subscribe(() => {
      alert(`Imagen subida a la propiedad ${propertyId}!`);
      this.selectedFile = null;
    });
  }

  listProperties() {
    this.service.listProperties().subscribe(props => {
      this.properties = props;
    });
  }
  updateProperty(p: PropertyDto) {
  this.service.updateProperty(p.idProperty!, p).subscribe({
    next: () => {
      alert(`Propiedad ${p.idProperty} actualizada!`);
      this.listProperties(); // refresca la lista
    },
    error: err => {
      console.error(err);
      alert('Error al actualizar la propiedad');
    }
  });
}

changePrice(p: PropertyDto) {
  const newPriceStr = prompt('Ingrese el nuevo precio:', p.price.toString());
  if (newPriceStr !== null) {
    const newPrice = Number(newPriceStr);
    if (!isNaN(newPrice)) {
      const changedBy = prompt('Ingrese su nombre para el registro de cambios:', 'Admin') || 'Admin';

      this.service.changePrice(p.idProperty!, newPrice, changedBy).subscribe({
        next: () => {
          alert(`Precio actualizado a ${newPrice} por ${changedBy}`);
          this.listProperties();
        },
        error: err => {
          console.error(err);
          alert('Error al cambiar el precio');
        }
      });
    } else {
      alert('Precio inválido');
    }
  }
}

showTraces(p: PropertyDto) {

  this.service.getTraces(p.idProperty!).subscribe(traces => {
  this.traces = traces;
  });
}
}