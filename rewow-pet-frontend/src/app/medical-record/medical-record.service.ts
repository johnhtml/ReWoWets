import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface PetOwner {
  id: number;
  name: string;
  pets: Pet[];
}

export interface Pet {
  id: number;
  name: string;
  type: string;
  size: string;
  description: string;
  ownerId: number;
  vaccinations: Vaccination[];
  medicalServices: MedicalService[];
}

export interface Vaccination {
  id: number;
  petId: number;
  number: string;
  type: string;
  date: string;
}

export interface MedicalService {
  id: number;
  petId: number;
  serviceType: string;
  date: string;
}

@Injectable({ providedIn: 'root' })
export class MedicalRecordService {
  private apiUrl = 'http://localhost:5019/api';

  constructor(private http: HttpClient) {}

  
  getPetOwners(): Observable<PetOwner[]> {
    return this.http.get<PetOwner[]>(`${this.apiUrl}/PetOwner`);
  }
  getPetOwner(id: number): Observable<PetOwner> {
    return this.http.get<PetOwner>(`${this.apiUrl}/PetOwner/${id}`);
  }
  createPetOwner(owner: Partial<PetOwner>): Observable<PetOwner> {
    return this.http.post<PetOwner>(`${this.apiUrl}/PetOwner`, owner);
  }
  updatePetOwner(id: number, owner: PetOwner): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/PetOwner/${id}`, owner);
  }
  deletePetOwner(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/PetOwner/${id}`);
  }

  
  getPets(): Observable<Pet[]> {
    return this.http.get<Pet[]>(`${this.apiUrl}/Pet`);
  }
  getPet(id: number): Observable<Pet> {
    return this.http.get<Pet>(`${this.apiUrl}/Pet/${id}`);
  }
  createPet(pet: Partial<Pet>): Observable<Pet> {
    return this.http.post<Pet>(`${this.apiUrl}/Pet`, pet);
  }
  updatePet(id: number, pet: Pet): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/Pet/${id}`, pet);
  }
  deletePet(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Pet/${id}`);
  }

  
  getVaccinations(): Observable<Vaccination[]> {
    return this.http.get<Vaccination[]>(`${this.apiUrl}/Vaccination`);
  }
  getVaccination(id: number): Observable<Vaccination> {
    return this.http.get<Vaccination>(`${this.apiUrl}/Vaccination/${id}`);
  }
  createVaccination(vaccination: Partial<Vaccination>): Observable<Vaccination> {
    return this.http.post<Vaccination>(`${this.apiUrl}/Vaccination`, vaccination);
  }
  updateVaccination(id: number, vaccination: Vaccination): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/Vaccination/${id}`, vaccination);
  }
  deleteVaccination(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Vaccination/${id}`);
  }

  
  getMedicalServices(): Observable<MedicalService[]> {
    return this.http.get<MedicalService[]>(`${this.apiUrl}/MedicalService`);
  }
  getMedicalService(id: number): Observable<MedicalService> {
    return this.http.get<MedicalService>(`${this.apiUrl}/MedicalService/${id}`);
  }
  createMedicalService(service: Partial<MedicalService>): Observable<MedicalService> {
    return this.http.post<MedicalService>(`${this.apiUrl}/MedicalService`, service);
  }
  updateMedicalService(id: number, service: MedicalService): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/MedicalService/${id}`, service);
  }
  deleteMedicalService(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/MedicalService/${id}`);
  }
} 