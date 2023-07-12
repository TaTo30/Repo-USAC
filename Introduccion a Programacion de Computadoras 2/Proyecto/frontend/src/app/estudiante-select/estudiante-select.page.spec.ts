import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EstudianteSelectPage } from './estudiante-select.page';

describe('EstudianteSelectPage', () => {
  let component: EstudianteSelectPage;
  let fixture: ComponentFixture<EstudianteSelectPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EstudianteSelectPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EstudianteSelectPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
