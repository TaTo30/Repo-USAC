import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignarAuxiliarPage } from './asignar-auxiliar.page';

describe('AsignarAuxiliarPage', () => {
  let component: AsignarAuxiliarPage;
  let fixture: ComponentFixture<AsignarAuxiliarPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AsignarAuxiliarPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AsignarAuxiliarPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
