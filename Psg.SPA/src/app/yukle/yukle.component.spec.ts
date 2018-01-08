/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { YukleComponent } from './yukle.component';

describe('YukleComponent', () => {
  let component: YukleComponent;
  let fixture: ComponentFixture<YukleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ YukleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(YukleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
