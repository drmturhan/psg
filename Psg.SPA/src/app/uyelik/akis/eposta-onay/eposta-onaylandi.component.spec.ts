/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { EpostaOnaylaComponent } from './eposta-onayla.component';

describe('EpostaOnaylaComponent', () => {
  let component: EpostaOnaylaComponent;
  let fixture: ComponentFixture<EpostaOnaylaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EpostaOnaylaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EpostaOnaylaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
