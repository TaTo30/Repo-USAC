import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignarCursoPage } from './asignar-curso.page';

describe('AsignarCursoPage', () => {
  let component: AsignarCursoPage;
  let fixture: ComponentFixture<AsignarCursoPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AsignarCursoPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AsignarCursoPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
