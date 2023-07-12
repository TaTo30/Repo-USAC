import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TabAuxiliarPage } from './tab-auxiliar.page';

describe('TabAuxiliarPage', () => {
  let component: TabAuxiliarPage;
  let fixture: ComponentFixture<TabAuxiliarPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TabAuxiliarPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TabAuxiliarPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
