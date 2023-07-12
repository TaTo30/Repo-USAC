import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TabCursosPage } from './tab-cursos.page';

describe('TabCursosPage', () => {
  let component: TabCursosPage;
  let fixture: ComponentFixture<TabCursosPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TabCursosPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TabCursosPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
