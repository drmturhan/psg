/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ArkadaslarimComponent } from './arkadaslarim.component';

describe('ArkadaslarimComponent', () => {
  let component: ArkadaslarimComponent;
  let fixture: ComponentFixture<ArkadaslarimComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArkadaslarimComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArkadaslarimComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
