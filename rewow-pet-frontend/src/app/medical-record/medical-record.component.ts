import { Component, OnInit } from '@angular/core';
import { MedicalRecordService, PetOwner, Pet, Vaccination, MedicalService } from './medical-record.service';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-medical-record',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './medical-record.component.html',
  styleUrls: ['./medical-record.component.scss']
})
export class MedicalRecordComponent implements OnInit {
  petOwners: PetOwner[] = [];
  selectedOwner: PetOwner | null = null;
  ownerForm: FormGroup;
  petForm: FormGroup;
  showOwnerForm = false;
  showPetForm = false;
  editingOwner: boolean = false;
  editingPet: boolean = false;
  selectedPet: Pet | null = null;
  vaccinationForm: FormGroup;
  medicalServiceForm: FormGroup;
  showVaccinationForm = false;
  showMedicalServiceForm = false;
  editingVaccination = false;
  editingMedicalService = false;
  currentVaccination: Vaccination | null = null;
  currentMedicalService: MedicalService | null = null;

  constructor(
    private service: MedicalRecordService,
    private fb: FormBuilder
  ) {
    this.ownerForm = this.fb.group({
      id: [0],
      name: ['', Validators.required]
    });
    this.petForm = this.fb.group({
      id: [0],
      name: ['', Validators.required],
      type: ['', Validators.required],
      size: ['', Validators.required],
      description: [''],
      ownerId: [0, Validators.required]
    });
    this.vaccinationForm = this.fb.group({
      id: [null],
      petId: [null, Validators.required],
      number: ['', Validators.required],
      type: ['', Validators.required],
      date: ['', Validators.required]
    });
    this.medicalServiceForm = this.fb.group({
      id: [null],
      petId: [null, Validators.required],
      serviceType: ['', Validators.required],
      date: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.loadOwners();
  }

  loadOwners() {
    this.service.getPetOwners().subscribe(owners => this.petOwners = owners);
  }

  selectOwner(owner: PetOwner) {
    this.service.getPetOwner(owner.id).subscribe(updatedOwner => {
      this.selectedOwner = updatedOwner;
      this.showOwnerForm = false;
      this.showPetForm = false;
      this.selectedPet = null;
    });
  }

  newOwner() {
    this.ownerForm.reset();
    this.showOwnerForm = true;
    this.editingOwner = false;
  }

  editOwner(owner: PetOwner) {
    this.ownerForm.patchValue(owner);
    this.showOwnerForm = true;
    this.editingOwner = true;
  }

  saveOwner() {
    if (this.editingOwner) {
      this.service.updatePetOwner(this.ownerForm.value.id, this.ownerForm.value).subscribe(() => {
        this.loadOwners();
        this.showOwnerForm = false;
      });
    } else {
      const { id, ...rest } = this.ownerForm.value;
      this.service.createPetOwner(rest).subscribe(() => {
        this.loadOwners();
        this.showOwnerForm = false;
      });
    }
  }

  deleteOwner(owner: PetOwner) {
    if (confirm('Delete this owner?')) {
      this.service.deletePetOwner(owner.id).subscribe(() => {
        this.loadOwners();
        if (this.selectedOwner?.id === owner.id) this.selectedOwner = null;
      });
    }
  }

  newPet(owner: PetOwner) {
    this.petForm.reset();
    this.petForm.patchValue({ ownerId: owner.id });
    this.showPetForm = true;
    this.editingPet = false;
  }

  editPet(pet: Pet) {
    this.petForm.patchValue(pet);
    this.showPetForm = true;
    this.editingPet = true;
  }

  savePet() {
    const petData = { ...this.petForm.value };
    
    if (!petData.id) {
      delete petData.id;
    }

    delete petData.owner;
    delete petData.vaccinations;
    delete petData.medicalServices;

    if (this.editingPet) {
      this.service.updatePet(petData.id, petData).subscribe(() => {
        if (this.selectedOwner) {
          this.selectOwner(this.selectedOwner);
          this.selectedOwner = { ...this.selectedOwner };
        }
        this.showPetForm = false;
      });
    } else {
      this.service.createPet(petData).subscribe(() => {
        if (this.selectedOwner) {
          this.selectOwner(this.selectedOwner);
          this.selectedOwner = { ...this.selectedOwner };
        }
        this.showPetForm = false;
      });
    }
  }

  deletePet(pet: Pet) {
    if (confirm('Delete this pet?')) {
      this.service.deletePet(pet.id).subscribe(() => {
        if (this.selectedOwner) this.selectOwner(this.selectedOwner);
      });
    }
  }

  selectPet(pet: Pet) {
    this.selectedPet = pet;
    this.showVaccinationForm = false;
    this.showMedicalServiceForm = false;
  }

  reloadSelectedPet() {
    if (this.selectedPet) {
      this.service.getPet(this.selectedPet.id).subscribe(pet => {
        this.selectedPet = pet;
        if (this.selectedOwner) {
          const idx = this.selectedOwner.pets.findIndex(p => p.id === pet.id);
          if (idx !== -1) {
            this.selectedOwner.pets[idx] = pet;
          }
        }
      });
    }
  }

  newVaccination(pet: Pet) {
    this.vaccinationForm.reset();
    this.vaccinationForm.patchValue({ petId: pet.id });
    this.showVaccinationForm = true;
    this.editingVaccination = false;
    this.currentVaccination = null;
  }

  editVaccination(vaccination: Vaccination) {
    this.vaccinationForm.patchValue({
      ...vaccination,
      petId: this.selectedPet?.id
    });
    this.showVaccinationForm = true;
    this.editingVaccination = true;
    this.currentVaccination = vaccination;
  }

  saveVaccination() {
    const vaccinationData = { ...this.vaccinationForm.value };
    if (!vaccinationData.id) delete vaccinationData.id;
    vaccinationData.petId = this.selectedPet?.id;
    if (this.editingVaccination && this.currentVaccination) {
      this.service.updateVaccination(this.currentVaccination.id, vaccinationData).subscribe(() => {
        this.reloadSelectedPet();
        this.showVaccinationForm = false;
      });
    } else {
      this.service.createVaccination(vaccinationData).subscribe(() => {
        this.reloadSelectedPet();
        this.showVaccinationForm = false;
      });
    }
  }

  deleteVaccination(vaccination: Vaccination) {
    if (confirm('¿Eliminar esta vacuna?')) {
      this.service.deleteVaccination(vaccination.id).subscribe(() => {
        this.reloadSelectedPet();
      });
    }
  }

  
  newMedicalService(pet: Pet) {
    this.medicalServiceForm.reset();
    this.medicalServiceForm.patchValue({ petId: pet.id });
    this.showMedicalServiceForm = true;
    this.editingMedicalService = false;
    this.currentMedicalService = null;
  }

  editMedicalService(service: MedicalService) {
    this.medicalServiceForm.patchValue({
      ...service,
      petId: this.selectedPet?.id
    });
    this.showMedicalServiceForm = true;
    this.editingMedicalService = true;
    this.currentMedicalService = service;
  }

  saveMedicalService() {
    const serviceData = { ...this.medicalServiceForm.value };
    if (!serviceData.id) delete serviceData.id;
    serviceData.petId = this.selectedPet?.id;
    if (this.editingMedicalService && this.currentMedicalService) {
      this.service.updateMedicalService(this.currentMedicalService.id, serviceData).subscribe(() => {
        this.reloadSelectedPet();
        this.showMedicalServiceForm = false;
      });
    } else {
      this.service.createMedicalService(serviceData).subscribe(() => {
        this.reloadSelectedPet();
        this.showMedicalServiceForm = false;
      });
    }
  }

  deleteMedicalService(service: MedicalService) {
    if (confirm('¿Eliminar este servicio médico?')) {
      this.service.deleteMedicalService(service.id).subscribe(() => {
        this.reloadSelectedPet();
      });
    }
  }
}
