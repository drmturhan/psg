/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { BulComponent } from './bul.component';

describe('BulComponent', () => {
  let component: BulComponent;
  let fixture: ComponentFixture<BulComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BulComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BulComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
