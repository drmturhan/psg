/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { UyeListesiComponent } from './uye-listesi.component';

describe('UyeListesiComponent', () => {
  let component: UyeListesiComponent;
  let fixture: ComponentFixture<UyeListesiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UyeListesiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UyeListesiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
