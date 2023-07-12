import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministradorPage } from './administrador.page';

describe('AdministradorPage', () => {
  let component: AdministradorPage;
  let fixture: ComponentFixture<AdministradorPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministradorPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministradorPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
