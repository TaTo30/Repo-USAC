import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CrearCursoPage } from './crear-curso.page';

describe('CrearCursoPage', () => {
  let component: CrearCursoPage;
  let fixture: ComponentFixture<CrearCursoPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CrearCursoPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CrearCursoPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
