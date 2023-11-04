import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Subject, debounceTime, map, of, switchMap } from 'rxjs';
import { UsersService } from 'src/app/services/users.service';
import { User } from '../../models/user';

@UntilDestroy()
@Component({
  selector: 'app-user-search',
  templateUrl: './user-search.component.html',
  styleUrls: ['./user-search.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserSearchComponent implements OnInit {
  public searchPhrase = new FormControl('');

  @Input() public users: User[] = [];
  @Output() public userAdded = new EventEmitter<User>();
  @Output() public userRemoved = new EventEmitter<User>();

  private filteredUsersSubject$ = new Subject<User[]>();
  public filteredUsers$ = this.filteredUsersSubject$.asObservable();

  private readonly debounceDelay = 500;

  constructor(
    private _usersService: UsersService
  ){}

  ngOnInit(): void {
    this.searchPhrase.valueChanges
      .pipe(
        untilDestroyed(this),
        debounceTime(this.debounceDelay),
        switchMap(value => value && value.length >= 3 ? this._usersService.getUsers(value) : of([])),
        map(usrs => usrs.filter(u => !this.users?.some(cu => cu.id == u.id))),
      )
      .subscribe(usrs => this.filteredUsersSubject$.next(usrs));
  }

  public selectionChange(select: User){
    if (select) {
      this.users = [
        ...this.users,
        select
      ];
      this.userAdded.emit(select);
      this.filteredUsersSubject$.next([]);
    }
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public userMapper(user: any) {
    return user?.name ?? '';
  }

  public removeUserFromSelected(user: User) {
    if (user) {
      this.users = this.users
        .filter(u => u.id != user.id);
      this.userRemoved.emit(user);
    }
  }
}
