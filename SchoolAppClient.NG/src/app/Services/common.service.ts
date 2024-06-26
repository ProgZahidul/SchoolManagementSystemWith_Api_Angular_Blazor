import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AcademicMonth } from '../Models/AcademicMonth';
import { Fee } from '../Models/Fee';
import { FeeType } from '../Models/FeeType';
import { MonthlyPayment } from '../Models/MonthlyPayment';
import { DueBalance } from '../Models/DeuBalance';

@Injectable({
  providedIn: 'root'
})
export class CommonServices {
  private apiUrl = 'https://localhost:7225/api/';
  private apiUrl3 = 'https://localhost:7225/api/Common';
  private apiUrl2 = 'https://localhost:7225/api/Common/Frequency';
  constructor(private http: HttpClient) { }

  // GET all academic months
  getAllAcademicMonths(): Observable<AcademicMonth[]> {
    const url = `${this.apiUrl}AcademicMonths`;  // Use apiUrl
    return this.http.get<AcademicMonth[]>(url);
  }
  getAllFees(): Observable<Fee[]> {
    const url = `${this.apiUrl}Fees`;
    return this.http.get<Fee[]>(url);
  }

  getAllFeeType(): Observable<FeeType[]> {
    const url = `${this.apiUrl}FeeTypes`;  // Replace with your actual API endpoint
    return this.http.get<FeeType[]>(url);
  }

  getFrequencyEnum(): Observable<string[]> {
    return this.http.get<string[]>(this.apiUrl2);
  }

  getAllStudents(): Observable<any[]> {
    const url = `${this.apiUrl}Students`;  // Replace with your actual API endpoint
    return this.http.get<any[]>(url);
  }

  getAllStandards(): Observable<any[]> {
    const url = `${this.apiUrl}Standards`;  // Replace with your actual API endpoint
    return this.http.get<any[]>(url);
  }

  getDueBalance(studentId: number): Observable<DueBalance> {
    return this.http.get<DueBalance>(`https://localhost:7225/api/Common/DueBalances/${studentId}`);
  }

  getAllPaymentsByStudentId(studentId: number): Observable<MonthlyPayment[]> {
    return this.http.get<MonthlyPayment[]>(`${this.apiUrl3}/GetAllPaymentByStudentId/${studentId}`);
  }
}
