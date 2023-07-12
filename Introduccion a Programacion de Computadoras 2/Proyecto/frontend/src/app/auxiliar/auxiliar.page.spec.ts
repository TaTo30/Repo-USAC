import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuxiliarPage } from './auxiliar.page';

describe('AuxiliarPage', () => {
  let component: AuxiliarPage;
  let fixture: ComponentFixture<AuxiliarPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuxiliarPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuxiliarPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
