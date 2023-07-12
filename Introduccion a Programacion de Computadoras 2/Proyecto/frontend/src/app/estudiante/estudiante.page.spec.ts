import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EstudiantePage } from './estudiante.page';

describe('EstudiantePage', () => {
  let component: EstudiantePage;
  let fixture: ComponentFixture<EstudiantePage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EstudiantePage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EstudiantePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
