/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ListelerComponent } from './listeler.component';

describe('ListelerComponent', () => {
  let component: ListelerComponent;
  let fixture: ComponentFixture<ListelerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListelerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListelerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
