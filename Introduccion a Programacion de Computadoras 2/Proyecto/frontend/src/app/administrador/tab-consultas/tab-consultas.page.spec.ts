import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TabConsultasPage } from './tab-consultas.page';

describe('TabConsultasPage', () => {
  let component: TabConsultasPage;
  let fixture: ComponentFixture<TabConsultasPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TabConsultasPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TabConsultasPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
