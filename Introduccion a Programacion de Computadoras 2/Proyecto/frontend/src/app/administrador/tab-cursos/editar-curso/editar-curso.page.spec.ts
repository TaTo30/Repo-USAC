import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditarCursoPage } from './editar-curso.page';

describe('EditarCursoPage', () => {
  let component: EditarCursoPage;
  let fixture: ComponentFixture<EditarCursoPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditarCursoPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditarCursoPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
