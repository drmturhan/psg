/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { UyelikComponent } from './uyelik.component';

describe('UyelikComponent', () => {
  let component: UyelikComponent;
  let fixture: ComponentFixture<UyelikComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UyelikComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UyelikComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
