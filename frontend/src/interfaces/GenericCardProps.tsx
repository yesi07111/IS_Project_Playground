export interface GenericCardProps {
  title: string;
  fields: { label: string; value: string | number }[];
  badge?: { text: string; color: string };
  actions?: { label: string; onClick: () => void }[];
}