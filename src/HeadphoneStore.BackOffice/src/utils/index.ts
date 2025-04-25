export const toTitleCase = (str: string) => {
  if (!str) return str

  return str
    .toLowerCase()
    .replace(/(^|\s)\w/g, (letter) => letter.toUpperCase())
}
