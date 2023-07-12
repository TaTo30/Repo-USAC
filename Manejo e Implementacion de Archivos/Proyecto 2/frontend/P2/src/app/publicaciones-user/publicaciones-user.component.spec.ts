import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicacionesUserComponent } from './publicaciones-user.component';

describe('PublicacionesUserComponent', () => {
  let component: PublicacionesUserComponent;
  let fixture: ComponentFixture<PublicacionesUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PublicacionesUserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PublicacionesUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
