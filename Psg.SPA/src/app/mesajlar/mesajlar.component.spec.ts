/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { MesajlarComponent } from './mesajlar.component';

describe('MesajlarComponent', () => {
  let component: MesajlarComponent;
  let fixture: ComponentFixture<MesajlarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MesajlarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MesajlarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
