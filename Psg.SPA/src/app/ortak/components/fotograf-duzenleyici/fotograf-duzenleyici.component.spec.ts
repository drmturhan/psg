/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FotografDuzenleyiciComponent } from './fotograf-duzenleyici.component';

describe('FotografDuzenleyiciComponent', () => {
  let component: FotografDuzenleyiciComponent;
  let fixture: ComponentFixture<FotografDuzenleyiciComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FotografDuzenleyiciComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FotografDuzenleyiciComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
