/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { VeriYuklenemediComponent } from './veri-yuklenemedi.component';

describe('VeriYuklenemediComponent', () => {
  let component: VeriYuklenemediComponent;
  let fixture: ComponentFixture<VeriYuklenemediComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VeriYuklenemediComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VeriYuklenemediComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
