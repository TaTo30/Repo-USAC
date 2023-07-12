import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TabAsignarPage } from './tab-asignar.page';

describe('TabAsignarPage', () => {
  let component: TabAsignarPage;
  let fixture: ComponentFixture<TabAsignarPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TabAsignarPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TabAsignarPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
