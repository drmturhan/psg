/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { UyelikBasariliComponent } from './uyelik-basarili.component';

describe('UyelikBasariliComponent', () => {
  let component: UyelikBasariliComponent;
  let fixture: ComponentFixture<UyelikBasariliComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UyelikBasariliComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UyelikBasariliComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
