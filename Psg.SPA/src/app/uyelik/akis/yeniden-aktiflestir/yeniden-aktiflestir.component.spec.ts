/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { YenidenAktiflestirComponent } from './yeniden-aktiflestir.component';

describe('YenidenAktiflestirComponent', () => {
  let component: YenidenAktiflestirComponent;
  let fixture: ComponentFixture<YenidenAktiflestirComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ YenidenAktiflestirComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(YenidenAktiflestirComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
