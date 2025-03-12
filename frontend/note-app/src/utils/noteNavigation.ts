import router from '@/router';

export function useNoteNavigation(customRouter?: any) {
  const routerInstance = customRouter || router;

  const navigateToNewNote = () => {
    routerInstance.push('/notes/new');
  };

  const navigateToNote = (id: string) => {
    routerInstance.push(`/notes/${id}`);
  };

  const navigateToNotesList = () => {
    routerInstance.push('/notes');
  };
  
  const goBack = () => {
    routerInstance.back();
  };

  return {
    navigateToNewNote,
    navigateToNote,
    navigateToNotesList,
    goBack
  };
}

